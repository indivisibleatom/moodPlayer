classdef Trainer
    properties (SetAccess = private)
        rootDirectory
    end

    methods
        function trainer = Trainer(trainDirectory)
            trainer.rootDirectory = trainDirectory;
        end
        
        function train(obj)
            cd(obj.rootDirectory);
            delete 'DS.wav'
            D = dir('*.wav');
            
            mean_Rms = zeros(numel(D),1);
            for x=1:numel(D)
                %% Read a wav file
                [y, Fs, nbits, readinfo] = wavread(D(x).name, 'double');
                wavwrite(y,16000,16,'DS.wav');% convert to 16khz, 16bits
                [y, Fs, nbits, readinfo] = wavread('DS.wav', 'double');
            
                %convert to mono
                y = (y(:,1)+y(:,2))/2;

                num_of_chops=floor(numel(y)/(10*Fs))-4;    % total samples in y/samples in 10s interval=no. of 10s intervals
                rmsSamples = zeros(1,num_of_chops);
                sample=zeros(1,10*Fs);

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
                mean_Rms(x,1)=mean(rmsSamples);
            end
            svmstruct=svmtrain(mean_Rms,[0;1],'showplot',false);
            save('trainingResult.mat','svmstruct');
        end
    end
end
