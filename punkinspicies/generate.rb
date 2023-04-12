#####
#  to run use:
#     $ ruby punkinspicies/generate.rb


require 'artfactory'




punkinspicies = Artfactory.read( './punkinspicies/spritesheet-24x24.png',
                                 './punkinspicies/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)

specs = parse_data( <<TXT )
  Frankensteins Monster, Bride, Choker, Vape, Bloodtears, Mole, Hoopearring
  Demon, Darkhair, Goldchain, Knife, Hoopearring
  Jack O Lantern, Bride, Bloodtears
TXT



specs.each_with_index do |attributes, i|
  img = punkinspicies.generate( *attributes )
  img.save( "./tmp/punkinspicies#{i}.png" )
  img.zoom(4).save( "./tmp/punkinspicies#{i}@4x.png" )
end

puts "bye"

