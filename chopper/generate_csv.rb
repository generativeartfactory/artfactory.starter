#####
#  to run use:
#     $ ruby chopper/generate_csv.rb


require 'artfactory'



chopper = Artfactory.read( './chopper/spritesheet-24x24.png',
                             './chopper/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)


## try (re)generate via (token) metadata / attributes

specs = read_csv( './chopper/chopper.csv' )

specs.each_with_index do |rec, i|

  ## cut-off first id column and reverse order
  attributes = rec.to_a[1..-1].reverse
  attributes = attributes.map {|k,v| "#{k} : #{v}"}

  img = chopper.generate( *attributes )
  img.save( "./chopper/24x24/#{i}.png" )
  img.zoom(4).save( "./chopper/tmp/chopper#{i}@4x.png" )
end


puts "bye"

