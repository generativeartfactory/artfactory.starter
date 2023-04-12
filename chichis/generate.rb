#####
#  to run use:
#     $ ruby chichis/generate.rb


require 'artfactory'


## setup generator

chichis = Artfactory.read( './chichis/spritesheet-32x32.png',
                           './chichis/spritesheet-32x32.csv',
                             width: 32,
                             height: 32)


## try some "random" samples

specs = parse_data( <<TXT )
  Magenta, Jersey,  Lightning Bolt Earrings, Cbd Cig, Aviators, Beanie
  Silver, Suit, Simple Day, Smile, Xx, Rainbow
  Deep Teal, Stripes, Simple Day, Tongue Out, Cobain, Bald
TXT

specs.each_with_index do |attributes, i|
   img = chichis.generate( *attributes )
   img.save( "./tmp/chichis#{i}.png" )
   img.zoom(4).save( "./tmp/chichis#{i}@4x.png" )
end

puts "bye"


