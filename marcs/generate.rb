#####
#  to run use:
#     $ ruby marcs/generate.rb


require 'artfactory'


marcs = Artfactory.read( './marcs/spritesheet-24x24.png',
                         './marcs/spritesheet-24x24.csv',
                                      width: 24,
                                      height: 24)

specs = parse_data( <<TXT )
  Marc 4, Frumpy Hair, Green Shirt, Lasers, Cigarette
  Zombie, Green Eyes, Wild Hair, Mustache, Polarized
  Alien, Green Eyes, Gold Earring, Crazy Hair, Full Mustache Dark, Horned Rim Glasses
TXT


specs.each_with_index do |attributes, i|
  img = marcs.generate( *attributes )
  img.save( "./tmp/marcs#{i}.png" )
  img.zoom(4).save( "./tmp/marcs#{i}@4x.png" )
end

puts "bye"



