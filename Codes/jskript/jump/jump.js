var arr = new Array();
var fso0 = new ActiveXObject("Scripting.FileSystemObject");
var file0 = fso0.OpenTextFile("ansi.txt");
var line0 = file0.ReadAll();
file0.Close();
//WSH.echo(line0.length);
for (var i = 0; i < 256; ++i)
{
	arr[i] = line0.charAt(i);
}
var arr1 = new Array();
for (var i = 0; i < 256; ++i)
{
	arr1[arr[i]] = i;
}
var name = WScript.Arguments(0);
var intxt = WScript.Arguments(1);
var outtxt = WScript.Arguments(2);
var fso = new ActiveXObject("Scripting.FileSystemObject");
var file = fso.OpenTextFile(intxt.toString());
var line = file.ReadAll();
file.Close();
line = line.split('');
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
	var fso1 = new ActiveXObject("Scripting.FileSystemObject");
	var file1 = fso1.OpenTextFile(outtxt.toString(), 2, -2);
	file1.Write(answer);
	file1.Close();
}
/////////

if (name == 'decode')
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
	var fso1 = new ActiveXObject("Scripting.FileSystemObject");
	var file1 = fso1.OpenTextFile(outtxt.toString(), 2, -2);
	file1.Write(answer);
	file1.Close();
}