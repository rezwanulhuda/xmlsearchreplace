# XML Search and Replace
# Project Description
XmlSearchReplace is a free commandline utility to replace text inside an xml document. It gives the user control over where in the xml document the search should be done. The tool allows users to search and replace texts only inside xml element/attribute name and/or value.

# Project Details

XmlSearchReplace is a command line tool to search and replace text from an Xml document. You can also achieve this by doing a regular text search and replace in the xml file. However, you don't have much control over where in the Xml you want to perform the search on. It might be, you want only the texts inside elements to be renamed - but a text search cannot distinguish between an Xml tag or a value.

XmlSearchReplace tries to overcome this by allowing users to specify where they want to perform the search. Also, it's a command line utility and supports wild cards - which means you can perform the search in multiple files and from a batch file.

I started working with the tool when I sometimes needed to change some reference from .csproj files of Visual Studio and it was getting tiresome to try and change 20-30 project references manually. I also wanted to practice some TDD - and started the development with a TDD approach.

I believe, apart from being a tool to replace texts from within xmls, the search replace engine can also be used by developers as an independent library in their projects where they need such features.

# Usage
```
XmlSearchReplace.exe /S="search" [/R]="replace" /O=en,ev,an,av /F="C:\Files\*.xml" [/C] [/I] [/W] [/D] [/L] [/P]="C:\Files\params.txt
S - String to search in the xml. If string contains front slash (/) then the entire string must be surrounded by slash double quotes(\"). If " needs to be searched, then use \" with the entire string inside " (optional)
R - String to replace with in the xml. If string contains front slash (/) then the entire string must be surrounded by slash double quotes(\"). If " needs to be searched, then use \" with the entire string inside " (optional)
O - Options for searching. Can have the following values: 
ev - element value
en - element name
an - attribute name
av - attribute value
va - value of attribute
ve - value of element
The va and ve options will search attribute/element name to match with search string supplied and will replace the value of the matching attribute/element.

Separate options with comma, semicolon or space

F - File name to perform search and replace on. Must be a valid xml file. Supports common windows wild cards * and ?. If wild cards are specified, searches all sub directories.
C - Continue on error when processing multiple files. (optional)
I - Ignore case. Default is case sensitive search. (optional)
W - Whole word search. When searching values, whole word means the entire value. (optional)
D - Recurse sub directories. (optional)
L - Replace the search string by a lower case version of the search string. (optional)
P - Specify file name containing multiple search and replace strings. Each line in file should contain a search and replace string. Only following parameters are accepted /S, /R and /L. Syntax is same as command line. (optional)
```

```
Example:

library.xml

<Library>
  <Books>
    <Book id="1">
      <Title>Refactoring: Improving the Design of Existing Code</Title>
      <Author>Martin Fowler</Author>
    </Book>
    <book id="1">
      <Title>Agile Principles, Patterns, and Practices in C#</Title>
      <Author>Martin C. Robert, Martin Micah</Author>
    </book>
  </Books>
</Library>
```

To replace all element names with the value "Book" with "BookInLibrary" on the file library.xml use the following command:
```
XmlSearchAndReplace.exe /O=en /S=Book /R=BookInLibrary /F=library.xml
```
Or
```
XmlSearchAndReplace.exe /O=en /S="Book" /R="BookInLibrary" /F="library.xml"
```

This will replace only the element named 'Book' but not the one named 'book'. To replace both, add the /I option to the above command line.
Similarly, you can use /O=ev,en to search for the text in both element name and value or /O=ev,av to search in element and attribute values.

If you use /F=*.xml - it will search through the directory and replace all files containing the extension xml in current directory and subdirectories.

To use front slash (/) in search and replace strings, use the following syntax:
```
XmlSearchAndReplace.exe /O=en /S="cars/trucks" /R="Cars/Trucks" /F="library.xml"
```

To use a double quote in either search or replace strings, use the following syntax:
```
XmlSearchAndReplace.exe /O=en /S="trucks with \"double\" chamber" /R="trucks with \"Double\" chamber" /F="library.xml"
```

To replace the search string with lower case, use the /L option.
```
XmlSearchAndReplace.exe /O=en /S=Book /F=library.xml /L will replace all string Book with book. The /R option if provided with the /L string will be ignored.
```

To specify multiple search and replace strings, you can optionally specify a parameter file using the /P option. The parameter file supports only /S, /R and /L options. If a parameter file is specified, then /S, /R and /L options in command lines are ignored. The parameter file has the same syntax as the command line. Each line in a parameter file should contain 1 /S /R or /S /L pair.
```
XmlSearchAndReplace.exe /O=en /F=library.xml /P=c:\list.txt will take all search and replace parameters from the paramter file c:\list.txt
```

Example content of list.txt
```
/S=Hello /L
/S=world /R=WORLD
```


To replace the value of an element that matches search string with the replace string use the following:
```
XmlSearchAndReplace.exe /O=ve /S=Title /R=Magic /L will replace all value of elements with the name Title with with word Magic replacing existing values.
```
Prior to replacement, a backup of the file is taken.
