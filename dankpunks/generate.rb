#####
#  to run use:
#     $ ruby dankpunks/generate.rb


require 'artfactory'


dankpunks = Artfactory.read( './dankpunks/spritesheet-24x24.png',
                             './dankpunks/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)


specs = parse_data( <<TXT )
  Pale,   Clown Nose, Red Punk, Green Clown
  Tan,    Black Hair, 3D Glasses
  Zombie, Black Mohawk, Dark Clown, Ok, Jay Z
TXT


specs.each_with_index do |attributes, i|
  img = dankpunks.generate( *attributes )
  img.save( "./tmp/dankpunks#{i}.png" )
  img.zoom(4).save( "./tmp/dankpunks#{i}@4x.png" )
end

puts "bye"

