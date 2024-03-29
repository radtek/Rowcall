﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TokenAPI.Models;
using TokenAPI.Data;
using SOAPservice;
using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;

namespace TokenAPI.Controllers
{
    [Route("api/[controller]")]
    public class TokensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokensController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // POST: api/Tokens
        [HttpPost]
        public async Task< IActionResult > PostToken([FromBody] TokenDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WebService1SoapClient client = new WebService1SoapClient
                (
                 new BasicHttpBinding(BasicHttpSecurityMode.None),
                 new EndpointAddress("http://localhost/SOAPTokenGenerator/TokenGenerator.asmx")
                 );


            var token = new Token()
            {
                Duration = 30,
                ClassId = dto.ClassId,
                CreatedDateTime = DateTime.Now,
                TokenValue = client.GenerateToken()
            };

            _context.Token.Add(token);
            _context.SaveChanges();

            AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient("AKIAJLAJIOHNR4Q2EQSQ", "+5t2ISSTpRYQnZomhd+C4S9LqQ8YiRqKjV1YRHLM", Amazon.RegionEndpoint.USWest2);
            var topicArn = await snsClient.CreateTopicAsync(new CreateTopicRequest(dto.ClassId.ToString()));

            String msg = "Your token for todays class is: " + token.TokenValue;
            PublishRequest publishRequest = new PublishRequest(topicArn.TopicArn, msg);
            var publishResult = await snsClient.PublishAsync(publishRequest);
            //print MessageId of message published to SNS topic

            return CreatedAtAction("GetToken", new { id = token.Id }, token);
        }

        [HttpGet("{id}")]
        public IActionResult GetTokens(int id)
        {
            var tokens = _context.Token.Where(x => x.ClassId == id);
            return Json(tokens); 
        }

        [Route("getstudentsfortoken/{tokenId}")]
        [HttpGet]
        public async Task<IActionResult> GetStudentsForToken(int tokenId)
        {
            var token = _context.Token.First(y => y.Id == tokenId);

            var classRoom = await _context.ClassRoom
                .Include(x => x.Students)
                .ThenInclude(x => x.ApplicationUser)
                .SingleOrDefaultAsync(x => x.Id == token.ClassId);

            var students = classRoom.Students.Select(x => x.ApplicationUser).ToList();

            //var userToken = _context.UserToken.First(x => x.TokenId == tokenId);

            var checkedInStudents = _context.UserToken.Where(x => x.TokenId == tokenId).Include(x => x.ApplicationUser).Select(x => x.ApplicationUser); 
                
            //    _context.Users.Select(y => y.UserTokens
            //.First(x => x.ApplicationUserId == userToken.ApplicationUserId).ApplicationUser)
            //.ToList();

            var missingStudents = students.Except(checkedInStudents).ToList(); 

            return Json(new{
                            presentStudents = checkedInStudents.Select(x => x.Email),
                            notPresentStudents = missingStudents.Select(x => x.Email)
                            });
        }
    }
}