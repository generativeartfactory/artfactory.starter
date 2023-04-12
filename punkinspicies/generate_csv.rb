#####
#  to run use:
#     $ ruby punkinspicies/generate_csv.rb


require 'artfactory'




punkinspicies = Artfactory.read( './punkinspicies/spritesheet-24x24.png',
                                 './punkinspicies/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)

## try (re)generate via (token) metadata / attributes

specs = read_csv( './punkinspicies/punkinspicies.csv' )

specs.each_with_index do |rec, i|

  attributes = rec.to_a
  ## cut-off first id column and reverse order
  attributes = attributes[1..-1].reverse
  attributes = attributes.map {|k,v| "#{k} : #{v}"}

  img = punkinspicies.generate( *attributes )
  img.save( "./punkinspicies/24x24/#{i}.png" )
  img.zoom(4).save( "./punkinspicies/tmp/punkinspicies#{i}@4x.png" )
end


puts "bye"
