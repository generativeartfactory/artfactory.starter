#####
#  to run use:
#     $ ruby inversepunks/generate.rb


require 'artfactory'




inversepunks = Artfactory.read( './inversepunks/spritesheet-24x24.png',
                                './inversepunks/spritesheet-24x24.csv',
                                      width: 24,
                                      height: 24)

specs = parse_data( <<TXT )
  Alien, Pimple, Cap Forward, Eyes, Bubble Gum, White Gold Chain, Gold Earring
  Alien, Clear, Knitted Cap, VR, White Gold Chain, Gold Earring
  Ape, Clear, Bandana, Classic Shades, Blue Medical Mask, Pink Sapphire Necklace
TXT


specs.each_with_index do |attributes, i|
  img = inversepunks.generate( *attributes )
  img.save( "./tmp/inversepunks#{i}.png" )
  img.zoom(4).save( "./tmp/inversepunks#{i}@4x.png" )
end


puts "bye"
