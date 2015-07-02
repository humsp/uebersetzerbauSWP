# Twee2Z
A compiler for converting **Twee source code** (http://twinery.org/) into **Z-code**(Version 8) (https://en.wikipedia.org/wiki/Z-machine).
The resulting Z-code can be executed with Z-machines like Frotz (http://frotz.sourceforge.net/).

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
    ANTLR4  : http://www.antlr.org/
    Mono    : http://www.mono-project.com/

##Example test files
    https://github.com/humsp/uebersetzerbauSWP/tree/develop/Twee2Z/TestFiles/UnitTestFiles

##Features
- passages
- functions:    
    - random
- links
- variables: short, boolean
- macro
    - set
    - print
    - display
    - if elsif else
- expressions exclude string concatenation

##History
Twee2Z was developed by students as part of the course "Softwareprojekt Übersetzerbau" in 2015