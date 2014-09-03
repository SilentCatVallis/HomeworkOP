var name = WScript.Arguments(0),
	fso = new ActiveXObject("Scripting.FileSystemObject"),
	file = fso.OpenTextFile("in.txt"),
	text = file.ReadAll();
file.Close();
text = text.split("\n");

var out = fso.OpenTextFile("out.txt", 2, -2);

var minConst = Math.pow(2, -126)

function MakeZeroAndOne(string, sign)
{
	if (isNaN(parseFloat(string)))
	{
		out.WriteLine("0 11111111 11111111111111111111111" + '\n');
		return '#'
	}
	var line = "",
		flag = 0,
		q = Math.pow(2, 128);
	while (string > 0 || q > 1)
	{
		if (q < minConst)
			break;
		if (string < 1 && flag == 0 && q <= 1)
		{
			line = line.concat(",");
			flag = 1;
		}
		q /= 2;
		if (q <= string)
		{
			string -= q;
			line = line.concat("1");
		}
		else
		{
			line = line.concat("0");
		}
	}
	if (string > Math.pow(2, -126))
	{
		out.WriteLine(sign + ' ' + "11111111 00000000000000000000000" + '\n');
		return "#";
	}
	else if (string != 0)
	{
		line = "0 00000000 ";
		for (var j = 0; j < 23; ++j)
		{
			q /= 2;
			if (q <= string)
			{
				string -= q;
				line = line.concat("1");
			}
			else
			{
				line = line.concat("0");
			}
		}
		out.WriteLine(line + '\n');
		return "#";
	}
	return line;
}

function MakeMantissa(number)
{
	var answer = "";
	var flag = 0;
	for (var i = 0; i < number.length; ++i)
	{
		if (flag == 1 && number.charAt(i) != ',')
			answer = answer.concat(number.charAt(i));
		else
		{
			if (number.charAt(i) == '1')
				flag = 1;
		}
	}
	return answer;
}

function MakeExponent(string)
{
	var exponent;
	var	firstDelims = string.indexOf(",");
	var	firstOne = string.indexOf("1");
	if (firstOne == -1)
	{
		return "00000000";
	}
	if (firstDelims == -1)
	{
		exponent = string.length - firstOne - 1;
	}
	else
	{
		exponent = firstDelims - firstOne - 1;
		if (firstDelims < firstOne)
		{
			exponent += 1;
		}
	}
	var answer = MakeMantissa(MakeZeroAndOne(127 + exponent), 0);
	var answer = "1".concat(answer);
	for (var j = 0; j < 8 - answer.length; ++j)
		answer = "0".concat(answer);
	return answer;
}

function CorrectMantissa(string)
{
	string = string.substr(0, 23);
	var amountOfStep = 23 - string.length;
	for (var i = 0; i < amountOfStep; ++i)
	{
		string = string.concat("0");
	}
	return string;
}


if (name == "conv")
{
	var number,
		exponent,
		mantissa;
	for (var i = 0; i < text.length; ++i)
	{
		
		var sign = 0;
		if (text[i] < 0)
		{
			sign = 1;
			text[i] *= -1;
		}
		if (text[i] == 0)
		{
			exponent = "00000000";
			mantissa = CorrectMantissa("0");
		}
		else
		{
			number = MakeZeroAndOne(text[i], sign);
			if (number == "#")
				continue;
			exponent = MakeExponent(number);
			mantissa = CorrectMantissa(MakeMantissa(number));
		}
		out.WriteLine(sign +' '+ exponent +' '+ mantissa + '\n');
	}
}

function Calc(number)
{
	var ans = 0;
	var count = number.length;
	for (var i = 0; i < count; ++i)
	{
		ans += parseInt(number.charAt(i)) * Math.pow(2, count - 1 - i);
	}
	return ans;
}

function Corrector(exponent, mantissa)
{
	var answer;
	var exp = Calc(exponent) - 127;
	if (exp >= 0)
	{
		var l1 = mantissa.substring(0, exp);
		var l2 = mantissa.substring(exp + 1, mantissa.length - 1);
		mantissa = l1.concat(",", l2);
		mantissa = "1".concat(mantissa);
	}
	return mantissa;
}

function CorrectLength(num1, num2)
{
	var delims1 = num1.indexOf(",");
	if (delims1 == -1)
		delims1 = 0;
	var delims2 = num2.indexOf(",");
	if (delims1 == -1)
		delims2 = 0;
	for (var i = 0; i < delims2 - delims1; ++i)
		num1 = "0".concat(num1);
	for (var i = 0; i < delims1 - delims2; ++i)
		num2 = "0".concat(num2);
	for (var i = 0; i < (num2.length - delims2) - (num1.length - delims1); ++i)
		num1 = num1.concat("0");
	for (var i = 0; i < (num1.length - delims1) - (num2.length - delims2); ++i)
		num2 = num2.concat("0");
	var answer = [];
	var bigger = 1;
	for (var i = 0; i < num1.length; ++i)
	{
		if (num1.charAt(i) != num2.charAt(i))
		{
			if (num1.charAt(i) == "0")
				bigger = 2;
			break;
		}
	}
	if (bigger == 1)
	{
		answer[0] = num1;
		answer[1] = num2;
	}
	else
	{
		answer[0] = num2;
		answer[1] = num1;
	}
	answer[2] = bigger - 1;
	return answer;
}

function Plus(num1, num2)
{
	var list = CorrectLength(num1, num2);
	num1 = list[0].split("");
	num2 = list[1].split("");
	var answer = new Array(num1.length + 1);
	var last = 0;
	var local = 0;
	for (var i = num1.length - 1; i >= 0; i--)
	{
		if (num1[i] != ",")
		{
			local = parseInt(num1[i]) + parseInt(num2[i]) + last;
			last = 0;
			if (local >= 2)
			{
				last = 1;
				local -= 2;
			}
			answer[i + 1] = local;
		}
		else
			answer[i + 1] = num1[i];
	}
	answer[0] = last;
	return answer.join("");
}

function Mins(num1, num2)
{
	var list = CorrectLength(num1, num2);
	sign = list[2];
	num1 = list[0].split("");
	num2 = list[1].split("");
	//WSH.echo(sign, num1, num2);
	var answer = new Array(num1.length);
	var last = 0;
	var local = 0;
	for (var i = num1.length - 1; i >= 0; i--)
	{
		if (num1[i] != ",")
		{
			local = parseInt(num1[i]) - parseInt(num2[i]) - last;
			last = 0;
			if (local < 0)
			{
				last = 1;
				local += 2;
			}
			answer[i] = local;
		}
		else
			answer[i] = num1[i];
	}
	var ans = [];
	ans[0] = list[2];
	ans[1] = answer.join("");
	return ans;
}
function MakeOperation(num1, num2, sign1, sign2, operator)
{	
	var sign0 = parseInt(sign1);
	var preAns,
		ans;
	if (sign1 == sign2)
	{
		if (operator == "+")
			ans = Plus(num1, num2);
		else
		{
			preAns = Mins(num1, num2);
			sign0 = (parseInt(sign0) + parseInt(preAns[0])) % 2;
			ans = preAns[1];
		}
	}
	else
	{
		if (operator == "+")
		{
			preAns = Mins(num1, num2);
			sign0 = (sign0 + preAns[0]) % 2;
			ans = preAns[1];
		}
		else
			ans = Plus(num1, num2);
	}
	var answer = [];
	answer[0] = sign0;
	answer[1] = ans;
	return answer;
}

/////////////////////////////////

if (name == "sum")
{
	if (text[2] == "+")
		out.WriteLine(+(text[0]) + +(text[1]) + '\n');
	else
		out.WriteLine(+(text[0]) - +(text[1]) + '\n');
	var number = [],
		exponent = [],
		mantissa = [],
		sign = [];
	for (var i = 0; i < 2; ++i)
	{
		sign[i] = 0;
		if (text[i] < 0)
		{
			sign[i] = 1;
			text[i] *= -1;
		}
		if (text[i] == 0)
		{
			exponent[i] = "00000000";
			mantissa[i] = CorrectMantissa("0");
		}
		else
		{
			number[i] = MakeZeroAndOne(text[i], sign[i]);
			if (number[i] == '#')
			{
				WScript.Quit();
			}
			exponent[i] = MakeExponent(number[i]);
			mantissa[i] = CorrectMantissa(MakeMantissa(number[i]));
		}
	}
	var operator = text[2];
	for (var i = 0; i < 2; ++i)
	{	
		number[i] = Corrector(exponent[i], mantissa[i]);
	}
	
	var preAnswer = MakeOperation(number[0], number[1], sign[0], sign[1], text[2]);
	line = preAnswer[1];
	signLine = preAnswer[0];
	expLine = MakeExponent(line);
	mantLine = CorrectMantissa(MakeMantissa(line));
	out.WriteLine(signLine + ' ' + expLine + ' ' + mantLine + '\n');
}