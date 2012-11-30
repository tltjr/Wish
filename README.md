Wish
====
This repository is the prototype for Wish, a project designed to provide a fully-featured command line replacement for Windows. Wish was originally a Kickstarter project you can still find here: [http://www.kickstarter.com/projects/20258791/wish](http://www.kickstarter.com/projects/20258791/wish). There is a video demo on the Kickstarter page which provides a nice overview of the features.  

If you would like to download the prototype, please see [Downloads](https://github.com/tltjr/Wish/downloads).  

This project is a prototype, and as such, has some good core code which can contribute towards a finished product, but also has many rough edges. Some portions of the prototype were hacked together to help build a video demo.  

Here are some of the major open items:  
1. The UI for the prototype is hacked together on top of AvalonEdit, a text editor. A proper user control needs to be developed.  
2. The Powershell engine simply sends off commands and returns the results. It needs to be redone as a custom Powershell host. Microsoft provides some excellent documentation for this and some of their code samples are contained in my repository [PShellHostSamples](git@github.com:tltjr/PShellHostSamples.git).  
3. Similarly, the cmd.exe engine starts a new process for each command and returns the results.

If you would like to help with any of these issues, please let me know.

