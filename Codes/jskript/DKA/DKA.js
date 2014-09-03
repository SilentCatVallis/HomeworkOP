var pattern, intxt, amount;
if (WScript.Arguments.length >= 2)
{
	pattern = WScript.Arguments(0);
	intxt = WScript.Arguments(1);
	amount = Infinity;
}
else
{
	WSH.echo("incorrect data, please try again");
	WScript.Quit();
}
if (WScript.Arguments.length > 2)
{
	amount = WScript.Arguments(2);
	amount++;
}
var fso = new ActiveXObject("Scripting.FileSystemObject");
if (fso.FileExists(intxt) == false)
{
	WScript.StdOut.WriteLine("file not found, please try again");
	WScript.Quit();
}
var	file = fso.OpenTextFile(intxt.toString()),
	line = file.ReadAll();
file.Close();

var letters = 0;

function createAutomaton(str) 
{ 
    var alphabet = [], 
        automaton = []; 
    for(var i = 0; i < str.length; i++) 
    { 
        alphabet[str.charAt(i)] = 1; 
        automaton[i] = []; 
    } 
    automaton[str.length] = []; 
	for (var i in alphabet)
		letters++; 
    for(var letter in alphabet) 
        automaton[0][letter] = 0;  
    for(var state = 0; state < str.length; state++) 
    { 
        var prev = automaton[state][str.charAt(state)]; 
        automaton[state][str.charAt(state)] = state + 1; 
        for(var letter in alphabet)
            automaton[state + 1][letter] = automaton[prev][letter]; 
    } 
    return automaton; 
} 
  
function printAutomaton(automaton) 
{ 
    var output = ['#']; 
    for(var letter in automaton[0]) 
        output.push(letter); 
	output.push(" ");
    WSH.Echo(output.join(' | ')); 
	var delims = "";
	for (var i = 0; i < 2 * (letters * 2 + 1); ++i)
		delims = delims.concat("_");
    WSH.Echo(delims); 
    for(var state = 0; state < automaton.length; state++) 
    { 
        var output = [state]; 
        for(var letter in automaton[state]) 
            output.push(automaton[state][letter]);
		output.push(" ");
        WSH.Echo(output.join(' | ')); 
		WSH.echo(delims);
    } 
}
  
var dataStart = new Date();
  
var auto = createAutomaton(pattern.toString());

var answer = [],
	status = 0;
	count = 0;

for (var i = 0; i < line.length; ++i)
{
	if (pattern.indexOf(line.charAt(i)) != -1)
	{
		status = auto[status][line.charAt(i)];
		if (status == pattern.length)
		{
			count++;
			answer[count] = i - pattern.length + 1;
		}
	}
	else
	{
		status = 0;
	}
}

var dataEnd = new Date();


printAutomaton(auto);
WSH.echo();
WSH.echo("count of pattern in text ", count);
for (var i = 0; i < answer.length && i < amount; ++i)
{
	WSH.echo(answer[i]);
}
WSH.echo();
WSH.echo("Time ", dataEnd - dataStart);