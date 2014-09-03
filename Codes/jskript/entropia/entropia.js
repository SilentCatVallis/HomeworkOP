var a = WSH.StdIn.ReadLine();
var b = new Array()
var n = 0;
var count = 0;
for (var i = 0; i < a.length; ++i)
{
	count++;
	if (a.charAt(i) in b)
		b[a.charAt(i)]++;
	else
	{
		b[a.charAt(i)] = 1;
		n++;
	}
}
var en = 0.0;
if (n == 1)
{
	for (var i in b)
	{
		if (b[i] == 1)
			WSH.echo(0);
		else
			WSH.echo(1);
	}		
}
else
{
	for (var i in b)
	{
		en = en + (Math.log(b[i] / count) / Math.log(n)) * (b[i] / count);
	}
	WSH.echo(-1.0 * en);
}
	