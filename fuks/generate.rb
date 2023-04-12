#####
#  to run use:
#     $ ruby fuks/generate.rb


require 'artfactory'


fuks = Artfactory.read( './fuks/spritesheet-24x24.png',
                        './fuks/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)

specs = parse_data( <<TXT )
  Robot Fuk, Shaved, Purple Hair, Nose, Vape, VR, No Earrings, Diamond Chain
  Undead Fuk, Shaved, Messy Hair, Nose, Smile, Laser, Diamond Stud, Silver Chain
  Human Fuk, Chin Strap, Fast Food Cap, Nose, Grin, Nerd Glasses, No Earrings, No Chains
TXT



specs.each_with_index do |attributes, i|
  img = fuks.generate( *attributes )
  img.save( "./tmp/fuks#{i}.png" )
  img.zoom(4).save( "./tmp/fuks#{i}@4x.png" )
end

puts "bye"


