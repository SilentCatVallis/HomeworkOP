var fso = new ActiveXObject("Scripting.FileSystemObject"),
	file = fso.OpenTextFile("example.txt"),
	line = file.ReadAll();
file.close();
line = line.toLocaleLowerCase();

var fileLibrary = fso.OpenTextFile("ansi.txt");
var lineLibrary = fileLibrary.ReadAll();
fileLibrary.Close();

var alphabet = [];
for (var i = 0; i < 59; ++i)
{
	alphabet[lineLibrary.charAt(i)] = 0;
}
var table = [];

var count = 0;
for (var i = 0; i < line.length; ++i)
{
	if (line.charAt(i) in alphabet)
	{
		count++;
		if (line.charAt(i) in table)
			table[line.charAt(i)]++;
		else
			table[line.charAt(i)] = 1;
	}
}
var fileOut = fso.OpenTextFile("stat.txt", 2, -2);
for (var i in table)
{
	fileOut.WriteLine(i + ' ' + table[i]/count);
}
	
	
	