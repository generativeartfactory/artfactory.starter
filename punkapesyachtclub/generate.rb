#####
#  to run use:
#     $ ruby punkapesyachtclub/generate.rb


require 'artfactory'



punkapesyachtclub = Artfactory.read( './punkapesyachtclub/spritesheet-24x24.png',
                                     './punkapesyachtclub/spritesheet-24x24.csv',
                                      width: 24,
                                      height: 24)

specs = parse_data( <<TXT )
  Black, Prison Jumpsuit, Top Hat, Green Eye Shadow, Cigarette With Hot Lipstick, Silver Loop
  Trippy, Biker Vest, Silver Chain, Pink With Hat, Clown Eyes Green, Frown, Silver Stud
  Zombie, Tanktop, Orange Side, 3D Glasses, Frown
TXT

specs.each_with_index do |attributes, i|
  img = punkapesyachtclub.generate( *attributes )
  img.save( "./tmp/punkapesyachtclub#{i}.png" )
  img.zoom(4).save( "./tmp/punkapesyachtclub#{i}@4x.png" )
end


puts "bye"