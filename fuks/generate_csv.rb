#####
#  to run use:
#     $ ruby fuks/generate_csv.rb


require 'artfactory'


fuks = Artfactory.read( './fuks/spritesheet-24x24.png',
                        './fuks/spritesheet-24x24.csv',
                                  width: 24,
                                  height: 24)


## try (re)generate via (token) metadata / attributes

specs = read_csv( './fuks/fuks.csv' )

specs.each_with_index do |rec, i|

  ## cut-off first id column and reverse order
  attributes = rec.to_a[1..-1].reverse
  attributes = attributes.map {|k,v| "#{k} : #{v}"}

  img = fuks.generate( *attributes )
  img.save( "./fuks/24x24/#{i}.png" )
  img.zoom(4).save( "./fuks/tmp/fuks#{i}@4x.png" )
end


puts "bye"



