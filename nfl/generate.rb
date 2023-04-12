#####
#  to run use:
#     $ ruby nfl/generate.rb


require 'artfactory'




nfl = Artfactory.read( './nfl/spritesheet-23x23.png',
                       './nfl/spritesheet-23x23.csv',
                         width: 23,
                         height: 23)

specs = parse_data( <<TXT )
  Bot, Oakland, 13, Full Beard Light
  Base 5, Atlanta, 7, Full Beard Black
  Zombie, Los Angeles 2, 13, Full Beard Brown
TXT



specs.each_with_index do |attributes, i|
  img = nfl.generate( *attributes )
  img.save( "./tmp/nfl#{i}.png" )
  img.zoom(4).save( "./tmp/nfl#{i}@4x.png" )
end


puts "bye"
