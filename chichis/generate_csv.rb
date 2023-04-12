#####
#  to run use:
#     $ ruby chichis/generate_csv.rb


require 'artfactory'


## setup generator

chichis = Artfactory.read( './chichis/spritesheet-32x32.png',
                           './chichis/spritesheet-32x32.csv',
                             width: 32,
                             height: 32)


## try (re)generate via (token) metadata / attributes

specs = read_csv( './chichis/chichis.csv' )

specs.each_with_index do |rec, i|

  ## cut-off first id column and reverse order
  attributes = rec.to_a[1..-1].reverse
  attributes = attributes.map {|k,v| "#{k} : #{v}"}

  img = chichis.generate( *attributes )
  img.save( "./chichis/32x32/#{i}.png" )
  img.zoom(4).save( "./chichis/tmp/chichis#{i}@4x.png" )
end


puts "bye"