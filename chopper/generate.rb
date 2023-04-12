#####
#  to run use:
#     $ ruby chopper/generate.rb


require 'artfactory'



chopper = Artfactory.read( './chopper/spritesheet-24x24.png',
                             './chopper/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)


specs = parse_data( <<TXT )
  AQUA, HAT 22, SMOKER, 3D, BANANA
  AP3, HAT : CHOPPER, NON-SMOKER, BIG SHADES, GOLDEN
  MIDNIGHT, HAT 10, SMOKER, CLASSIC SHADES, DIAMOND CROSS
TXT


specs.each_with_index do |attributes, i|
  img = chopper.generate( *attributes )
  img.save( "./tmp/chopper#{i}.png" )
  img.zoom(4).save( "./tmp/chopper#{i}@4x.png" )
end


puts "bye"


