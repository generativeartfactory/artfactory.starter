#####
#  to run use:
#     $ ruby dankpunks/generate_csv.rb


require 'artfactory'


dankpunks = Artfactory.read( './dankpunks/spritesheet-24x24.png',
                             './dankpunks/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)


## try (re)generate via (token) metadata / attributes

specs = read_csv( './dankpunks/dankpunks.csv' )

specs.each_with_index do |rec, i|

  ## cut-off first id column and reverse order
  attributes = rec.to_a[1..-1].reverse
  attributes = attributes.map {|k,v| "#{k} : #{v}"}

  img = dankpunks.generate( *attributes )
  img.save( "./dankpunks/24x24/#{i}.png" )
  img.zoom(4).save( "./dankpunks/tmp/dankpunks#{i}@4x.png" )
end


puts "bye"
