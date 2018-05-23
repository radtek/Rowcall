# Roll-call System
This system was created as a final project of 1. semester for Development of Large Systems and System Integration

It's purpose is to register attendance of students in classes. System that is being used today is unsuitable for this task
as it is teacher himself who has to do all the work by going through each individual student and checking if he is present.

System created by us helps teacher by delegating most of the work to students. It works by teacher having multiple
classes where each class has number of students. Teacher can generate a unique token that will belong to one class
and every student that is in that class can use that token + his KEA credentials to check-in to the class.

We have implemented several functions preventing student from simply loging in too late or from home. When student 
wants to login system always checks:
- If token value is valid
- If token is still valid (every token has 30 min duration)
- If student is in certain area from school 


# Deployment Instructions
### Requirements
- Open 'RowcallBackend' project console and run 'update-database' command
- Then we also need to know is the system type that the project will be deployed on (Windows/OSX/Linux)



