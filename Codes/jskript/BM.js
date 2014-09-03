var pattern = WScript.Arguments(0),
	input = WScript.Arguments(1);
var count = Infinity;
if (WScript.Arguments.length > 2)
	count = WScript.Arguments(2);
var fso = new ActiveXObject("Scripting.FileSystemObject"),
	file = fso.OpenTextFile(input.toString()),
	text = file.ReadAll();
file.Close();



function PreffixFunction (string) 
{	
	WSH.echo(string);
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

var Pi = PreffixFunction(pattern.toString());
WSH.echo(Pi);
var textLength = text.length;
var n = pattern.length
var k = n;
var ansArray = [];
var amount = -1;
for (var i = n - 1; i < textLength; ++i)
{
	if (text.charAt(i - (n - k)) == pattern.charAt(k))
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
		for (var j = n - k; j >= 0; j--)
		{
			if (Pi[j] > 0)
			{
				i += Pi[j] - 1;
				break;
			}
		}
		i++;
	}
}
	