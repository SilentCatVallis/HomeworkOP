if (WScript.Arguments.length < 3)
{
	WSH.echo("incorrect data, please try again");
	WScript.Quit();
}
var input = WScript.Arguments(0),
	output = WScript.Arguments(1),
	type = WScript.Arguments(2);
if (type == 'encode')
{
	if (WScript.Arguments.length != 4)
	{
		WSH.echo("incorrect data, please try again");
		WScript.Quit();
	}
	var step = WScript.Arguments(3);
}

var fso = new ActiveXObject("Scripting.FileSystemObject");
if (fso.FileExists(input) == false)
{
	WScript.StdOut.WriteLine("file not found, please try again");
	WScript.Quit();
}	
var	file = fso.OpenTextFile(input.toString()),
	line = file.ReadAll();
file.close();
line = line.toLocaleLowerCase();

var fileLibrary = fso.OpenTextFile("ansi.txt");
var lineLibrary = fileLibrary.ReadAll();
fileLibrary.Close();

var alphabet = [];
for (var i = 0; i < 59; ++i)
{
	alphabet[lineLibrary.charAt(i)] = i;
}
var index = [];
int ip = -1;
for (var i in alphabet)
{
	ip++;
	index[ip] = i;
}

var filestat = fso.OpenTextFile("stat.txt");
var statline = filestat.ReadAll();
filestat.Close();
statline = statline.split('\n');

var GT = [];
for (var i = 0; i < statline.length; ++i)
{
	var local = statline[i].split(' ');
	GT[local[0]] = local[1];
}

var answer = [];

if (type == "encode")
{
	var l = -1;
	for (var i = 0; i < line.length; ++i)
	{
		if (line.charAt(i) in alphabet)
		{
			l++;
			answer[l] = index[(alphabet[line.charAt(i)] + step) % alphabet.length];
		}
	}
	fileOut.WriteLine(answer.join(''));
}
else if (type == "decode")
{
	var LT = [];
	for (var i = 0; i < line.length; ++i)
	{
		if (line.charAt(i) in alphabet)
		{
			if (line.charAt(i) in LT)
				LT[line.charAt(i)]++;
			else
				LT[line.charAt(i)] = 1;
		}
	}
	for (var i in LT)
	{
		table[i] /= line.length;
	}
	var ans = Infinity;
	var ind = -1;
	for (var i = 0; i < alphabet.length; ++i)
	{	
		var total = 0;
		var flag = 0;
		for (var j = 0; j < alphabet.length; ++j)
		{
			if (index[(i + j) % index.length] in GT)
			{
				flag = 1;
				if (index[j] in LT)
					total += GT[index[(i + j) % index.length]] - LT[index[j]];
				else
					total += GT[index[(i + j) % index.length]];
			}
		}
		if (total < ans && flag == 1)
		{
			ans = total;
			ind = i;
		}
	}
}
else
{
	WScript.StdOut.WriteLine("incorrect data, please try again");
	WScript.Quit();
}	