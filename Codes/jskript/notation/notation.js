function preority(a) 
{
    if (a == '+' || a == '-')
        return 2;
    else if (a == '*' || a == '/')
        return 3;
    else if (a == '^')
        return 4;
    else if (a == '(' || a == ')')
        return 1;
	else return 100;
}
var name;
if (WScript.Arguments.length > 0)
	name = WScript.Arguments(0);
else
{
	WSH.Echo('Enter name of procedure, please');
	name = WSH.StdIn.ReadLine();
}
if (name == "in2post")
{
	WSH.echo("hello, i'm translate standart form into Polish notation, write you expression");
	var str = WScript.StdIn.ReadLine();
	var fl = 0;
	var err = 0;
	str = str.split(' ');
	str = str.join('');
	str = str.split('');
	for (var i = 0; i < str.length; i++)
	{
		if (str[i] == "(" && str[i + 3] == ")")
		{
			if (str[i + 1] == '-' || str[i + 1] == '+')
				str.splice(i, 4, "(".concat(str[i+1], str[i+2], str[i+3]));
			else
			{
				err = 1;
			}
		}
	}
	var open = 0;
	for (var i = 0; i < str.length; ++i)
	{
		if (str[i] == "(")
		open++;
		if (str[i] == ")")
		open--;
		if (open < 0)
		{
			if (fl != 1) WSH.echo("ERROR statement");
			err = 1;
		}
	}
	if (open > 0)
	{
		if (fl != 1) WSH.echo("ERROR statement");
		err = 1;
	}
	var letter = 0;
	var operator = 0;
	for (var i = 0; i < str.length; ++i)
	{
		if (str[i] != "(" && str[i] != ")")
		{
			if (str[i] != "*" && str[i] != "/" && str[i] != "-" && str[i] != "+" && str[i] != "(" && str[i] != ")" && str[i] != "^") 
				letter++;
			else
				operator++;
		}
	}
	if (operator + 1 != letter)
	{
		if (fl != 1) WSH.echo("ERROR statement");
		err = 1;
	}	
	var answer = new Array();
	var stack = new Array();
	function pre(a) {
		if (a == '+' || a == '-')
			return 2;
		else if (a == '*' || a == '/')
			return 3;
		else if (a == '^')
			return 4;
		else if (a == '(' || a == ')')
			return 1;
	else return 100;
	}
	function Preority (l)
	{
		if (stack[stack.length - 1] == "(") {
			return 0;
		}
		if (stack[stack.length - 1] == ")") {
			var quantity = 0;
			for (var i = stack.length - 1; i >= 0; i--) {
				quantity++;
				if (stack[i] == "(") {
					break;
				}
			}
		return quantity - 1;
		}
		else
		{
			var quantity = 0;
			for (var i = stack.length - 2; i >= 0; i--) {
					if (stack[stack.length - 1] != "^")
					{
						if (pre(stack[stack.length - 1]) <= pre(stack[i])) {
							quantity++;
						}
						else {
							break;
						}
					}
					else
					{
						if (pre(stack[stack.length - 1]) < pre(stack[i])) {
							quantity++;
						}
						else {
							break;
						}
					}
			}
		return quantity;
		}
	}
	for (var i = 0; i < str.length; ++i) {
		if (str[i] != "*" && str[i] != "/" && str[i] != "-" && str[i] != "+" && str[i] != "(" && str[i] != ")" && str[i] != "^") {
			answer.push(str[i]);
		}
		else {
			stack.push(str[i]);
			var amount = Preority(i);
			var t = stack[stack.length - 1];
			stack.pop();
			for (var j = 0; j < amount; ++j) {
				if (stack[stack.length - 1] != ")" && stack[stack.length - 1] != "(")
					answer.push(stack[stack.length - 1]);
				stack.pop();
			}
			if (t != ')')
				stack.push(t);
		}
	}
	if (stack.length > 0) {
		for (var i = 1; i <= stack.length; ++i) {
		if (stack[stack.length - i] != ")" && stack[stack.length - i] != "(")
				answer.push(stack[stack.length - i]);
		}
	}
	if (err == 0)
		if (fl != 1) WSH.echo(answer.join(""));
}
else if (name == "post2in")
{
	WSH.echo("Hello, i'm translate from Polish notation into standart form, write you expression");
	var str = WScript.StdIn.ReadLine();
	str = str.split(' ');
	str = str.join('');
	str = str.split('');
	var err = 0;
	for (var i = 0; i < str.length; i++)
	{
		if (str[i] == "(" && str[i + 3] == ")")
		{
			if (str[i + 1] == '+' || str[i + 1] == '-')
				str.splice(i, 4, "(".concat(str[i+1], str[i+2], str[i+3]));
			else
				err = 1;
		}
	}
	var answer = new Array();
	var letter = 0;
	var operator = 0;
	for (var i = 0; i < str.length; ++i)
	{
		if (str[i] != "(" && str[i] != ")")
		{
			if (str[i] != "*" && str[i] != "/" && str[i] != "-" && str[i] != "+" && str[i] != "(" && str[i] != ")" && str[i] != "^") 
				letter++;
			else
				operator++;
		}
		if (operator + 1 > letter)
		{
			err = 1;
		}
	}
	if (operator + 1 != letter)
	{
		err = 1;
	}
	var lastPreor = new Array();
	if (err != 1)
	{
		for (var i = 0; i < str.length; ++i)
		{
			if (str[i] == '-' || str[i] == '+' || str[i] == '*' || str[i] == '/' || str[i] == '^')
			{
				var a = answer[answer.length - 1];
				answer.pop();
				var b = answer[answer.length - 1];
				answer.pop();
				var c = "(";
				b = b.toString();
				a = a.toString();
				var ap = lastPreor[lastPreor.length - 1];
				lastPreor.pop();
				var bp = lastPreor[lastPreor.length - 1];
				lastPreor.pop();
				if (ap < preority(str[i]))
				{
					a = c.concat(a, ")");
				}
				if (bp < preority(str[i]))
				{
					b = c.concat(b, ")");
				}
				b = b.concat(str[i], a);
				answer.push(b);
				lastPreor.push(preority(str[i]));
			}
			else
			{
				answer.push(str[i]);
				lastPreor.push(110);
			}
		}
		answer = answer.join("");
	}
	if (err == 0)
		WSH.echo(answer);
	else
		WSH.echo("ERROR");	
}
else
	WSH.echo("error statement");