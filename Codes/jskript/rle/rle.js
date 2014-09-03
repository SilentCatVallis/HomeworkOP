var arr = new Array();
var fso = new ActiveXObject("Scripting.FileSystemObject");
var fileLibrary = fso.OpenTextFile("ansi.txt");
var lineLibrary = fileLibrary.ReadAll();
fileLibrary.Close();
//WSH.echo(line0.length);
for (var i = 0; i < 256; ++i)
{
	arr[i] = lineLibrary.charAt(i);
}
var arr1 = new Array();
for (var i = 0; i < 256; ++i)
{
	arr1[arr[i]] = i;
}
if (WScript.Arguments.length != 4)
{
	WSH.echo("incorrect data, please try again");
	WScript.Quit();
}
var type = WScript.Arguments(0);
var name = WScript.Arguments(1);
var intxt = WScript.Arguments(2);
var outtxt = WScript.Arguments(3);
if (fso.FileExists(intxt) == false)
{
	WScript.StdOut.WriteLine("file not found, please try again");
	WScript.Quit();
}
var file = fso.OpenTextFile(intxt.toString());
var line = file.ReadAll();
file.Close();
line = line.split('');

if (type == 'escape')
{
	if (name == 'encode')
	{
		var local = line[0];
		var count = 0;
		var answer = '';
		for (var i in line)
		{
			if (line[i] == local && count < 259)
			{
				count++;
			}
			else
			{
				if (count > 3)
				{
					if (local != '#')
					{
						answer = answer.concat('#', arr[count - 4], local);
					}
					else
					{
						answer = answer.concat('#', arr[count], local);
					}
					count = 1;
				}
				else
				{
					if (local != '#')
					{
						for (var j = 0; j < count; ++j)
						{
							answer = answer.concat(local);
						}
					}
					else
					{
						answer = answer.concat('#', arr[count], local);
					}
					count = 1;
				}
				local = line[i];
			}
			//WSH.echo(count, local);
		}
		if (count > 0)
		{
			if (count > 3)
			{
				if (local != '#')
				{
					answer = answer.concat('#', arr[count - 4], local);
				}
				else
				{
					answer = answer.concat('#', arr[count], local);
				}
				count = 1;
			}
			else
			{
				if (local != '#')
				{
					for (var j = 0; j < count; ++j)
					{
						answer = answer.concat(local);
					}
				}
				else
				{
					answer = answer.concat('#', arr[count], local);
				}
				count = 1;
			}
			local = line[i];
		}
		var fileOut = fso.OpenTextFile(outtxt.toString(), 2, -2);
		fileOut.Write(answer);
		fileOut.Close();
	}
	else if (name == 'decode')
	{
		var answer = '';
		var i = 0;
		while (i < line.length)
		{
			if (line[i] == '#')
			{
				if (i + 1 < line.length)
				{
					if (line[i + 2] != '#')
					{
						for (var j = 0; j < arr1[line[i + 1]] + 4; ++j)
						{
							answer = answer.concat(line[i + 2]);
						}
						i += 3;
					}
					else
					{
						for (var j = 0; j < arr1[line[i + 1]]; ++j)
						{
							answer = answer.concat(line[i + 2]);
						}
						i += 3;
					}
				}
				else
				{
					//WSH.echo("ERROR");
				}
			}
			else
			{
				answer = answer.concat(line[i]);
				i++;
			}
		}
		var fileOut = fso.OpenTextFile(outtxt.toString(), 2, -2);
		fileOut.Write(answer);
		fileOut.Close();
	}
	else
	{
		WScript.StdOut.WriteLine("incorrect data, please try again");
		WScript.Quit();
	}
}







else if (type == 'jump')
{
	/////////
	line[line.length] = '$';
	if (line[line.length - 1] == line[line.length - 2])
	{
		line[line.length - 1] = '@';
	}
	//WSH.echo(line);
	if (name == 'encode')
	{
		var local = line[0];
		var answer = "";
		var i = 0;
		var count = 0;
		var fl = 0;
		while (i < line.length - 1)
		{
			//WSH.echo(fl);
			if (fl == 0)
			{
				if (line[i] != line[i + 1])
				{
					fl = 2;
				}
				else
				{
					fl = 1;
				}
			}
			if (fl == 2)
			{
				if (line[i + 1] != line[i] && count < 128)
				{
					i++;
					count++;
					//WSH.echo(line[i]);
				}
				else
				{
					//WSH.echo(WAT);
					answer = answer.concat(arr[count + 127]);
					for (var j = 0; j < count; ++j)
					{
						answer = answer.concat(line[i - (count - j)]);
					}
					fl = 0;
					count = 0;
				}
	
			}
			if (fl == 1)
			{
				if (line[i] == line[i + 1] && count < 128)
				{
					count++;
					i++;
					//WSH.echo(line[i]);
				}
				else
				{
					//WSH.echo(WAT);
					answer = answer.concat(arr[count - 1]);
					answer = answer.concat(line[i]);
					fl = 0;
					count = 0;
					i++;
				}
			}
		}
		if (count > 0)
		{
			if (fl == 2)
			{
					answer = answer.concat(arr[count + 127]);
					for (var j = 0; j < count; ++j)
					{
						answer = answer.concat(line[i - (count - j)]);
					}
	
			}
			if (fl == 1)
			{
	
					answer = answer.concat(arr[count - 1]);
					answer = answer.concat(line[i]);
			}
		}
		//WSH.echo(answer);
		var fileOut = fso.OpenTextFile(outtxt.toString(), 2, -2);
		fileOut.Write(answer);
		fileOut.Close();
	}
	/////////
	
	else if (name == 'decode')
	{
		var answer = "";
		var i = 0;
		var count;
		var fl = 0;
		while (i < line.length)
		{
			if (fl == 0)
			{
				count = line[i];
				i++;
				if (arr1[count] < 128)
					fl = 1;
				else
					fl = 2;
			}
			else
			{
				if (fl == 1)
				{
					for (var j = 0; j < arr1[count] + 2; ++j)
						answer += line[i];
					i++;
				}
				else
				{
					for (var j = 0; j < arr1[count] - 127; ++j)
					{
						answer += line[i];
						i++;
					}			
				}
				fl = 0;
			}
		}
		//WSH.echo(answer);
		var fileOut = fso.OpenTextFile(outtxt.toString(), 2, -2);
		fileOut.Write(answer);
		fileOut.Close();
	}
	else
	{
		WScript.StdOut.WriteLine("incorrect data, please try again");
		WScript.Quit();
	}
}
else
{
	WScript.StdOut.WriteLine("incorrect data, please try again");
	WScript.Quit();
}