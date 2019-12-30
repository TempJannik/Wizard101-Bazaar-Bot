# Wizard101BazaarBot

[How it looks](https://i.gyazo.com/20d588d821d789c65dff8e4a60f89c12.mp4)


## Disclaimer
I made this in my free time and barely have time to keep it up to date, if any issues arise either fix them yourself or create an issue on Github and I might be able to fix it eventually if your error description is sufficient.
The bot is not flawless sometimes it still bugs out or fails to recognize something.

Depending on Feedback maybe I'll add the Treasure Card functionality to it. If you are a programmer and wish to do this feel free to do so and make a pull request.

## What does it do?
This Wizard101 Bazaar Bot will automatically buy rare reagents for you. You can select which reagents you want it to search for in the Bazaar.

## How does it work?
When starting the bot assumes you are in the reagents bazaar window. It will then proceed to refresh the reagents window and then screenshot the list of reagents. It then compares the screenshot of the list to a screenshot of the reagent you want to buy, if it is found it will buy it automatically. Afterwards it will skip to the next page and so on. Everything is based on Image Recognition and Coordinates.

## Requirements
* .Net Framework: This Project is in C# so it requires the .Net Framework. This is included in most Windows Installations but if it is not there you can download it from [here](https://www.microsoft.com/de-de/download/details.aspx?id=55170)
**You need version 4.7.2 or higher!**
* 1080p Monitor: As the project relies on image recognition, if your monitor has less than 1080p Pixels or if your game isnt set to 1920x1080 the pixels will differ rendering the bot useless.

## How to install
* Press on the Releases button
* Click on the .rar attachment to download it or click [here](https://github.com/Tempodoa/Wizard101BazaarBot/releases/download/1.2/Release.rar)
* Extract the rar file into any Folder
* Start Wizard101BazaarBot.exe

## How to use
* Put your game into Fullscreen 1920x1080, Press Alt + Enter, your screen should now look like [this](https://gyazo.com/90b5f8e334bbd9eb1ebb0173236325fd)
* Go into the Bazaar, to the Reagents section
* Hover over the reagent button in W101, check the X and Y position of your mouse (top left of the bot), if it differs from the Reagent Position box under it change it and press Save
* Click your wanted Reagents from the Possible Reagents window and vice versa
* Press Start
* Apart from the Log, finished purchases will also be in the Purchases folder!

## How to kill
**This Bot will take Control of your Mouse!** For this reason you need to press **F1** to kill the bot.

## How to add new Reagents
It is entirely possible that reagents don't get recognized due to differences from PC to PC. In the Folder of the program you will also find a Folder named BazaarItems. This includes the images of the Reagents. If you want to add your own you must add a screenshot of it to this folder. It will automatically be in the Possible Reagents list on your next start of the Bot. To screenshot it you can use tools like Greenshot or Lightshot

## How to fix failed recognitions
In the BazaarUtils folder are 4 Images needed for the flow of the program to function. If anything fails I would recommend to retake these screenshots on your own PC. 
* isloading.png: This is from when the reagents get refreshed
* nextpage.png and nextpagegray.png: These are at the bottom of Wizard101 to navigate the list of reagents
* ok.png: This is the confirmation after purchasing a reagent

## Unfinished features
* Remaining Gold
* Removing screenshots from BazaarItems folder

## Contact
Discord: Temp#0712
