# DePot?
Pot is a discord bot that I (slab) created; originally just for rolling dice, but over time we've added more features...   
The whole project is still extremally buggy and unfinished, but we're getting closer than ever to finishing it.  
If any of ya have any funny ideas for auto replies, go ahead and edit the replies.json file   

# Pot Team
-Anthony slab (owner, developer)  
-Juna Meinhold (developer, the smart guy)   

# Support
If you like this bot and would want to see it develop, consider supporting me on patreon:  
https://www.patreon.com/anthonyslab?fan_landing=true  

# Build Requirements
.NET 6.0 SDK and Visual Studio 2022

# Features
Auto-Replies: the pot will reply with a set string to a message in chat that contains another set string;  
All of the replies are contained in the replies.json file  

Auto f-mee6: replies to any message sent by MEE6 with "FUCK YE BOT!"

Dad jokes: the pot will reply to any message in chat that contains "I'm __" with "hello __, I'm pot"

# Commands
[] - required  
{} - optional 

!gam [string]: sets the pot's status to "playing [string]" (requires the ViewAuditLog Guild permission)  

!say [string]: makes the pot say whatever string you input and then delete your message (requires the ManageMessages Guild permission) 
  
!uwuify [string]: makes ffe bot uwuify any stwing ffat you input, and dewete youw message (requires <MabageMessages Guild permission to do the latter)  
  
!roll {int}d[int] rolls dice, the first number is the amount of dice rolled and the second is the number of sides those dice have.  
Max allowed amount is 15 and the only accepted amounts of sides are 2,4,6,8,10,12,20,100  
after a successful roll the bot will return all of the rolls and the total  
  
!warn [memberId] [reason]: warns the user! and automatically executes an action, defferent to how many times the user was warned already. so:  
- 1st warn - timeout 10min  
- 2nd warn - timeout 1h  
- 3rd warn - kick from the guild    
- 4th warn - ban from the guild    
 
(requires the KickMembers Guild permission)  

!warns [memberId]: displays all of the warns that the user has with the exact date of their creation and reason (requires the ViewAuditLog Guild permission)  

!unwarn [memberId]: removes a warn from a specified member. note: it will not remove a timeout or a ban and that has to be done manually. (requires the KickMembers Guild permission)  

!sacredscrolls: enables the activity kick feature (requires the ManageWebhooks Guild permission)  

!profanescrolls: disabled the activity kick feature (requires the ManageWebhooks Guild permission)  

!godmode [roleName]: add an immune role which prevents all that have it from beung kicked due to inactivity (requires the ManageWebhooks Guild permission)  

!devilmode [roleName]: removes an immune role (requires the ManageWebhooks Guild permission)  

!markedtime [TimeSpan]: sets the time at which an inacive member will recieve a warning about their inactivity (requires the ManageGuild Guild permission) 

!sacredinfo: spits out a wall of info containing:  
- the status of inactivity kicking  
- warn delay after inactivity  
- kick delay after inactivity  
- immune roles to kicks from inactivity  
- all tracked users, the last time they were active, and if they were warned about inactivity or not  

# TODO List
-add more auto replies  

