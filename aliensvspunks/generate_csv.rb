#####
#  to run use:
#     $ ruby aliensvspunks/generate_csv.rb


require 'artfactory'


# step 1 - setup the art factory;
#    pass-in the spritesheet image & (meta) dataset and
#    the format (e.g. 24x24px)

aliensvsspunks = Artfactory.read( './aliensvspunks/spritesheet-24x24.png',
                                  './aliensvspunks/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)



## try (re)generate via (token) metadata / attributes

specs = read_csv( './aliensvspunks/aliensvspunks.csv' )

specs.each_with_index do |rec, i|

  ## cut-off first id column and reverse order
  attributes = rec.to_a[1..-1].reverse
  attributes = attributes.map {|k,v| "#{k} : #{v}"}

  img = aliensvsspunks.generate( *attributes )
  img.save( "./aliensvspunks/24x24/#{i}.png" )
  img.zoom(4).save( "./aliensvspunks/tmp/aliensvspunks#{i}@4x.png" )
end


puts "bye"
