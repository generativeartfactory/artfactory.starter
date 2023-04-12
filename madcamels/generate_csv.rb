#####
#  to run use:
#     $ ruby madcamels/generate_csv.rb


require 'artfactory'



madcamels = Artfactory.read( './madcamels/spritesheet-32x32.png',
                             './madcamels/spritesheet-32x32.csv',
                              width: 32,
                              height: 32)

## try (re)generate via (token) metadata / attributes

specs = read_csv( './madcamels/madcamels.csv' )

specs.each_with_index do |rec, i|

  attributes = rec.to_a
  ## cut-off first id column and reverse order
  attributes = attributes[1..-1].reverse
  attributes = attributes.map {|k,v| "#{k} : #{v}"}

  img = madcamels.generate( *attributes )
  img.save( "./madcamels/32x32/#{i}.png" )
  img.zoom(4).save( "./madcamels/tmp/madcamels#{i}@4x.png" )
end


puts "bye"
