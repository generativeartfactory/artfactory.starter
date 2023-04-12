#####
#  to run use:
#     $ ruby edgepunks/generate.rb



require 'artfactory'



edgepunks = Artfactory.read( './edgepunks/spritesheet-24x24.png',
                             './edgepunks/spritesheet-24x24.csv',
                             width: 24,
                             height: 24)

specs = parse_data( <<TXT )
  Dark Green Lizard, Dark Robe, Monster Mouth, Bat, Laser Eyes
  Purple Body, Dark Suit, Smile, Dark Hair, Dark Shades
  Whited Body, Monster Mouth, Edge Wizard Hat, Lizard Eyes
TXT


specs.each_with_index do |attributes, i|
   img = edgepunks.generate( *attributes )
   img.save( "./tmp/edgepunks#{i}.png" )
   img.zoom(4).save( "./tmp/edgepunks#{i}@4x.png" )
end


puts "bye"