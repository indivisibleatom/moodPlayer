classdef Classifier
    properties (SetAccess = private)
        classificationFile
        classificationDirectory
    end

    methods
        function classifier = Classifier(directory, file)
            classifier.classificationFile = file;
            classifier.classificationDirectory = directory;
        end
        
        function classify(obj)
            cd(obj.classificationDirectory);
            delete 'DS.wav'
            D = obj.classificationFile;
            
            mean_Rms = zeros(1,1);
            %% Read a wav file
            [y, Fs, nbits, readinfo] = wavread(D, 'double');
            wavwrite(y,16000,16,'DS.wav');% convert to 16khz, 16bits
            [y, Fs, nbits, readinfo] = wavread('DS.wav', 'double');
            
            %convert to mono
            y = (y(:,1)+y(:,2))/2;

            num_of_chops=floor(numel(y)/(10*Fs))-4;% total samples in y/samples in 10s interval=no. of 10s intervals
            rmsSamples = zeros(1, num_of_chops);
            sample=zeros(1, 10*Fs);

            starting_point=20*Fs;
            for i=1:num_of_chops
                for j=1:10*Fs
                    sample(j)=y(starting_point+j-1);
                end
                %operations on sample
                %Calculate RMS
                rmsSamples(i)=norm(sample)^2/numel(sample);
                starting_point=starting_point+10*Fs;
            end
            mean_Rms(1,1)=mean(rmsSamples);
        
            load('c:\Users\Mukul\Desktop\Computational Creativity\moodPlayer\moodPlayer\signalProcessing\training\trainingResult.mat','svmstruct');
            classes = svmclassify(svmstruct,mean_Rms,'showplot',false);
            delete 'DS.wav'

            fid = fopen('c:\Users\Mukul\Desktop\Computational Creativity\moodPlayer\moodPlayer\signalProcessing\testing\classificationResult.txt','a');
            formatSpec = '%s , %d\r\n';
            fprintf(fid,formatSpec,D,classes(1));
            fclose(fid);
        end
    end
end
