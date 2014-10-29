[2011-03-09]
I decided it's better to show in output (and as before) all details,
hiding them is just a kind of premature optimization. So, now it is:

------------------------------
    sky1.jpg       v1       v2
------------------------------
    sky1.jpg     0.00     0.00
    sky2.jpg     0.77     0.79
   lake1.jpg    14.28    15.07   <-  v1 < 20 but v2 is too big;
  bears2.jpg    23.29    19.65
  bears3.jpg    26.60    21.90
  roses2.jpg    29.41    14.08
  roses1.jpg    31.36    14.53
     ff1.jpg    33.47    16.55
  bears1.jpg    36.60    18.99
     ff2.jpg    39.52    18.60
  water2.jpg    39.69    10.35
  water3.jpg    39.80     9.09
  water1.jpg    40.11    10.37
------------------------------
Press...

So far the rule for estimation of v1 and v2 is as follows:

--- If v1 < 10 then almost for sure 2 pics are exact matches
    (as an example test pics water 1, 2, 3.jpg);

--- If v1 < 20 AND v2 < 10 the pics are very similar, maybe one of
    them is an artistic copy of another (roses2.jpg & roses1.jpg);

--- If v1 > 20-25 the pics are totally unrelated;



[2011-03-08]
Now ImSim's output looks like:

1st-line sifting [by fe()]
--------------------
sky1.jpg
--------------------
    sky1.jpg    0.00
    sky2.jpg    0.77
   lake1.jpg   14.28
--------------------


2nd-line sifting [by ff()]
--------------------
sky1.jpg
--------------------
    sky1.jpg
    sky2.jpg
--------------------

Press...



[2011-03-06]
----------------------
License: Public Domain
----------------------
ImSim is a python script for finding the most similar pic(s) to
a given one among a set/list/db of your pics.
The script is very short and very easy to follow and understand.
Its sample output looks like this:

  bears2.jpg
--------------------
  bears2.jpg    0.00
  bears3.jpg   15.37
  bears1.jpg   19.13
    sky1.jpg   23.29
    sky2.jpg   23.45
     ff1.jpg   25.37
   lake1.jpg   26.43
  water1.jpg   26.93
     ff2.jpg   28.43
  roses1.jpg   31.95
  roses2.jpg   36.12

Done!

The *less* numeric value -- the *more similar* this pic is to the
tested pic. If this value > 20 almost for sure these pictures are
absolutely different (from totally different domains, so to speak).

What is "similarity" and how can/could/should it be estimated this
point I'm leaving for your consideration/contemplation/arguing etc.

Several sample pics (*.jpg) are included into .zip.
And of course the stuff requires PIL (Python Imaging Library), see:
Home-page: http://www.pythonware.com/products/pil
Download-URL: http://effbot.org/zone/pil-changes-116.htm
