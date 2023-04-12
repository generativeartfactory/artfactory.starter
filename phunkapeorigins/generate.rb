#####
#  to run use:
#     $ ruby phunkapeorigins/generate.rb


require 'artfactory'



phunkapeorigins = Artfactory.read( './phunkapeorigins/spritesheet-24x24.png',
                                   './phunkapeorigins/spritesheet-24x24.csv',
                                      width: 24,
                                      height: 24)

specs = parse_data( <<TXT )
  Chimpanzee 1, Arrow, Snow Visor, Tooth Plug, Diamond Chain
  Silver Back, Wild Monkey Hair, Focused, Silver Dangle, Gold Chain, Cigar
  Yeti, Royal Crown, Matrix Shades, Silver Stud, Tooth Necklace, Tooth
TXT

specs.each_with_index do |attributes, i|
  img = phunkapeorigins.generate( *attributes )
  img.save( "./tmp/phunkapeorigins#{i}.png" )
  img.zoom(4).save( "./tmp/phunkapeorigins#{i}@4x.png" )
end

puts "bye"

