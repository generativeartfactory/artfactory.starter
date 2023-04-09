# Art Factory Quick Starter



## Getting Started w/ the Art Factory Gem

Yes, you can!  (Re)use the (pixel) artwork
spritesheets to generate any combination using text-to-image prompts
using the [**Art Factory machinery »**](https://github.com/generativeartfactory/artfactory)





### Collection №1 - Aliens vs Punks


Let's try the Aliens vs Punks:

``` ruby
require 'artfactory'

# step 1 - setup the art factory;
#    pass-in the spritesheet image & (meta) dataset and
#    the format (e.g. 24x24px)

aliensvsspunks = Artfactory.read( 'aliensvspunks/spritesheet-24x24.png',
                                  'aliensvspunks/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)


# step 2 - generate images via text (prompts)

specs = [
  ## no.37
  ['Solid Pink', 'Jacket : Grey', 'Blue Alien Girl', 'Red Kerchief',
   'Blue Buzz Cut', 'Brown', 'Pink Tiara'],
  ## no.28
  ['Solid Blue', 'Olive Guy', 'Orange Collar', 'Face Mask',
   'Yellow Ponytail', 'Green', 'Long Green'],
  ## no.21
  ['Solid Green', 'Jacket : White', 'Purple Alien Girl', 'Pink Collar', 'Neutral',
   'Pink Bob', 'Sunset Shades'],
]


specs.each_with_index do |attributes, i|
   img = aliensvsspunks.generate( *attributes )
   img.save( "aliensvspunks#{i}.png" )
   img.zoom(4).save( "aliensvspunks#{i}@4x.png" )
end
```

Voila!

![](i/aliensvspunks0.png)
![](i/aliensvspunks1.png)
![](i/aliensvspunks2.png)

4x:

![](i/aliensvspunks0@4x.png)
![](i/aliensvspunks1@4x.png)
![](i/aliensvspunks2@4x.png)



### Collection №2 - Edge Punks


Let's try the Edge Punks:

``` ruby
edgepunks = Artfactory.read( 'edgepunks/spritesheet-24x24.png',
                             'edgepunks/spritesheet-24x24.csv',
                             width: 24,
                             height: 24)

specs = [
# no.282
  ['Pink', 'Dark Green Lizard', 'Dark Robe', 'Monster Mouth', 'Bat', 'Laser Eyes'],
# no.468
  ['Pink', 'Purple Body', 'Dark Suit', 'Smile', 'Dark Hair', 'Dark Shades'],
# no.678 (1/1)
  ['Anatomy Of An Icon'],
]


specs.each_with_index do |attributes, i|
   img = edgepunks.generate( *attributes )
   img.save( "edgepunks#{i}.png" )
   img.zoom(4).save( "edgepunks#{i}@4x.png" )
end
```

Voila!

![](i/edgepunks0.png)
![](i/edgepunks1.png)
![](i/edgepunks2.png)

4x:

![](i/edgepunks0@4x.png)
![](i/edgepunks1@4x.png)
![](i/edgepunks2@4x.png)





### Collection №3 - Chi Chis

Let's try the Chi Chis:

``` ruby
chichis = Artfactory.read( 'chichis/spritesheet-32x32.png',
                           'chichis/spritesheet-32x32.csv',
                             width: 32,
                             height: 32)

specs = [
  ## no.15
  ['Wave', 'Magenta', 'Jersey',  'Lightning Bolt Earrings',
    'Cbd Cig', 'Aviators', 'Beanie'],
  ## no.8
  ['Palms', 'Silver', 'Suit', 'Simple Day', 'Smile', 'Xx', 'Rainbow'],
  ## 1/1
  ['Chichi Phunk'],
]

specs.each_with_index do |attributes, i|
   img = chichis.generate( *attributes )
   img.save( "chichis#{i}.png" )
   img.zoom(4).save( "chichis#{i}@4x.png" )
end
```

Voila!

![](i/chichis0.png)
![](i/chichis1.png)
![](i/chichis2.png)

4x:

![](i/chichis0@4x.png)
![](i/chichis1@4x.png)
![](i/chichis2@4x.png)





### And Many More Collections

Let's try some more...


**Chopper (in 24×24px)**

Samples:

- SOLID BLUE,  AQUA, HAT 22, SMOKER, 3D, BANANA
- SOLID GOLD, AP3, HAT CHOPPER, NON-SMOKER, BIG SHADES, GOLDEN
- SOLID BLACK, MIDNIGHT, HAT 10, SMOKER, CLASSIC SHADES, DIAMOND CROSS

![](i/chopper0.png)
![](i/chopper1.png)
![](i/chopper2.png)

4x:

![](i/chopper0@4x.png)
![](i/chopper1@4x.png)
![](i/chopper2@4x.png)



and so on.






## Questions? Comments?

Post them over at the [Help & Support](https://github.com/geraldb/help) page. Thanks.