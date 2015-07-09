# Twee2Z
A compiler for converting [Twee source code](http://twinery.org/) into [Z-code](https://en.wikipedia.org/wiki/Z-machine) (version 8) .
The resulting Z-code can be executed with Z-machines like [Frotz](http://frotz.sourceforge.net/).

### Usage

    Usage: -tw2z  <source> <destination> -log <arg1> <arg2>
    
     -tw2z      The code from the source language Twee will be translated to Z8 code.
                Input   : -tw2z <source> <destination>
                Example : -tw2z myTwee.tw zfile.z8
     -logAll    Activate all logs.
     -log       Activate specific logs. Possible arguments:
                all, analyzer, codegen, debug, objecttree, validation
                Input   : -log <arg1> <arg2> ...
                Example : -log analyzer debug
     -help      Display help message.
                Example usage: -tw2z myTwee.tw zfile.z8 -logAll

##Release location 
    TODO
##Libs
- ANTLR  : http://www.antlr.org/ (Version 4.4 Runtime 4.0)
- Mono    : http://www.mono-project.com/

##Example test files
https://github.com/humsp/uebersetzerbauSWP/tree/develop/Twee2Z/TestFiles/UnitTestFiles

##Features
- passages
- functions:    
    - random
    - visited
    - turns
- links
- variables: short, boolean
- macro
    - set
    - print
    - display
    - if elsif else
- expressions exclude string concatenation

##History
Twee2Z was developed by 8 students as part of the computer science course "SWP Compiler Engineering 2015" at FU-Berlin.
