classdef DataSetParser
    %UNTITLED2 Summary of this class goes here
    %   Detailed explanation goes here
    
    properties
    end
    
    methods
        function obj = DataSetParser()
        end
        
        function obj = getIntensities(a)
            import HDF5_Song_File_Reader
            index = 1;
            dirs = rdir('C:\Users\Mukul\Desktop\Computational Creativity\moodPlayer\moodPlayer\signalProcessing\MillionSongSubset\data\**\*.h5');
            for i = 1 : length(dirs)
                h5 = HDF5_Song_File_Reader(dirs(i).name);
                intensity = h5.get_artist_mbtags();
                if length(intensity) ~= 0
                    for j = 1 : length(intensity)
                        fprintf('%s\n', intensity{j,1});
                    end
                    %intensities(index) = intensity{j,1};
                    index = index+1;
                end;
            end;
            obj = zeros(1:11);
        end
    end
    
end

