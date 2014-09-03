var pattern = WScript.Arguments(0),
	input = WScript.Arguments(1);
var count = Infinity;
if (WScript.Arguments.length > 2)
	count = WScript.Arguments(2);
var fso = new ActiveXObject("Scripting.FileSystemObject"),
	file = fso.OpenTextFile(input.toString()),
	text = file.ReadAll();
file.Close();

var dataStart = new Date();

function PreffixFunction (string) 
{	
	//WSH.echo(string);
	var n = string.length;
	var PreffixArray = new Array(n);
	for (var k = 0; k <= n; ++k)
		PreffixArray[k] = 0;
	for (var i = 1; i < n; ++i) 
	{
		var j = PreffixArray[i - 1];
		while (j > 0 && string.charAt(i) != string.charAt(j))
			j = PreffixArray[j - 1];
		if (string.charAt(i) == string.charAt(j)) 
			++j;
		PreffixArray[i] = j;
	}
	return PreffixArray;
}

function Automaton (string)
{
	var alphabet = [];
	for (var i = 0; i < string.length; ++i)
	{
		alphabet[string.charAt(i)] = i + 1;
	}
	return alphabet;
}

var Pi = PreffixFunction(pattern.toString()),
	escape = Automaton(pattern.toString()),
	textLength = text.length,
	n = pattern.length,
	k,
	ansArray = [],
	amount = -1;
for (var i = n - 1; i < textLength; ++i)
{
	k = n;
	while (k > 0)	
	{
		if (text.charAt(i - (n - k)) == pattern.charAt(k - 1))
		{
			
			k--;
			if (k == 0)
			{
				amount++;
				ansArray[amount] = (i - n + k);
			}
		}
		else
		{
			
			var delta1 = 0;
			var delta2 = 0;
			if (text.charAt(i - (n - k)) in escape)
				delta2 = n - escape[text.charAt(i - (n - k))] - (n - k);
			else
				delta2 = n;
			if (delta2 <= 0)
				delta2 = 1;
			delta2 -= 1;
			for (var j = n; j >= 0; j--)
			{
				if (Pi[j] > 0 && Pi[j] <= n - k)
				{
					delta1 = Pi[j] - 1;
					break;
					
				}
			}
			var delta = delta1;
			if (delta2 > delta)
				delta = delta2;
				
			i += delta;
			break;
		}
	}
}

var dataEnd = new Date();
WSH.echo("Time ", dataEnd - dataStart);
WSH.echo();

WSH.echo(amount + 1);
for (var j = 0; j < count && j < ansArray.length ; ++j)
	WSH.echo(ansArray[j] + 1);