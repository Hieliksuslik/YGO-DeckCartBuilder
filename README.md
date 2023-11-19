# YGO-DeckCartBuilder

A little project for myself to practice minor webscraping.  
I have no intentioning of financial gain from this, just wanted a quick way to make the cheapest possible shopping cart for the decks I want to play.  
Data is pulled from https://yugiohprices.com/  

## Input:
Text File of cardnames.  
### Format:
One after another, no additional spaces at the end of each line.  

Dark Magician  
Timaeus the United Dragon  
The Bystial Lubellion  
Bystial Druiswurm  
Bystial Magnamhut  
Bystial Saronir  
Dark Magician Girl  
...  

## Output:
Text File for each cardname, with each card entry containing the vendor name, edition, rarity, condition, link-to-vendor-listing, and price.  

## Usage:
./YGO-DeckCartBuilder args[0] args[1]

args[0] = Path of input file. Ex: /home/Hielik/InputFile.txt  
args[1] = Path of output file. Ex: /home/Hielik/OutputFile.txt  

## NOTE:
This does not take into account the number of copies of each card you want.   
This is just to mainly get the links to each vendor in a single place for ease of shopping-cart creation.  

## TODO:
~~Add file output options. Likely have it as only csv output so that it's easier to parse through a spreadsheet. ~~  
Outputs a csv file alongside the user given text file
