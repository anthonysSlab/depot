# DePot?
Pot is a discord bot that I (slab) created; originally just for rolling dice, but over time we've added more features... 

The whole project is still extremally buggy and unfinished, but we're getting closer than ever to finishing it.

If any of ya hav eany funny ideas for auto replies, go ahead and edit the replies.json file

# Pot Team
-Anthony slab (owner, developer)

-Juna Meinhold (developer, the smart guy)

-Mutant031101 (test object)

# Support
If you like this bot and would want to see it develop, consider supporting me on patreon:
https://www.patreon.com/anthonyslab?fan_landing=true

# Features
[] - required
{} - optional

Auto-Replies: the pot will reply with a set string to a message in chat that contains another set string;
All of the replies are contained in the replies.json file

Auto f-mee6:
replies to any message sent by MEE6 with "FUCK YE BOT!"

Dad jokes: the pot will reply to any message in chat that contains "I'm __" with "hello __, I'm pot"

!serenademe [string]: sets the pot's status to "playing [string]" (requires the ManageWebhooks Guild permission)

!say [string]: makes the pot say whatever string you input and then delete your message (requires the ManageMessages Guild permission) 
  
!uwuify [string]: makes ffe bot uwuify any stwing ffat you input, and dewete youw message (fe wast pawt onwy accessibwe to me)
  
!roll {int}d[int] rolls dice, the first number is the amount of dice rolled and the second is the number of sides those dice have.
Max allowed amount is 15 and the only accepted amounts of sides are 2,4,6,8,10,12,20,100
after a successful roll the bot will return all of the rolls and the total
  
!badboi [userId] [reason]: (not finished :/)

# TODO List
-finish the !warn command
-add more auto replies
