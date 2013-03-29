clc
clear all
close all

cd 'C:\Users\Mukul\Desktop\Computational Creativity\moodPlayer\moodPlayer\signalProcessing\training';
delete 'DS.wav'
D = dir('*.wav');


for x=1:numel(D)
%% Read a wav file
[y, Fs, nbits, readinfo] = wavread(D(x).name, 'double');
wavwrite(y,16000,16,'DS.wav');% convert to 16khz, 16bits
[y, Fs, nbits, readinfo] = wavread('DS.wav', 'double');
%convert to mono
[r c]=size(y);
y = (y(:,1)+y(:,2))/2;
%y=y((1:r),1);

num_of_chops=floor(numel(y)/(10*Fs))-4;% total samples in y/samples in 10s interval=no. of 10s intervals

results=zeros(num_of_chops,2);

starting_point=20*Fs;
for i=1:num_of_chops
    sample=zeros(1, 10*Fs);
    for j=1:10*Fs
        sample(i,j)=y(starting_point+j-1);
    end
    %operations on sample
    %Calculate RMS
    R(i,1)=norm(sample)^2/length(sample);
    %sample_fft(i,:)=fft(sample(i,:));
    starting_point=starting_point+10*Fs;
end
 mean_Rms(x)=mean(R(:,1));

end
%mean_Rms=normr(mean_Rms);

TrainMat=[mean_Rms(1,:);zeros(1,16)];
T=[zeros(1,8) ones(1,8)];

%Rms(x)=sqrt(sum((y(1:r).^2))/numel(y));
cd 'C:\Users\Mukul\Desktop\Computational Creativity\moodPlayer\moodPlayer\signalProcessing\testing';
delete 'DS.wav';
D = dir('*.wav');
for x=1:numel(D)

[z, Fs, nbits, readinfo] = wavread(D(x).name, 'double');
wavwrite(z,16000,16,'DS.wav');% convert to 16khz, 16bits
[z, Fs, nbits, readinfo] = wavread('DS.wav', 'double');
%convert to mono
[r c]=size(z);
z = (z(:,1)+z(:,2))/2;
num_of_chops=floor(numel(z)/(10*Fs))-4;% total samples in y/samples in 10s interval=no. of 10s intervals

starting_point=20*Fs;
for i=1:num_of_chops
    sample=zeros(1, 10*Fs);
    for j=1:10*Fs
        sample(i,j)=z(starting_point+j-1);
    end
    %operations on sample
    %Calculate RMS
    R(i,1)=norm(sample)^2/length(sample);
    %sample_fft(i,:)=fft(sample(i,:));
     starting_point=starting_point+10*Fs;
end
 mean_Rms_tst(x)=mean(R(:,1));
end

%mean_Rms_tst=normr(mean_Rms_tst);
%Rms_z=sqrt(sum((z(1:r).^2))/numel(z));

svmStruct = svmtrain(TrainMat,T,'showplot',true);
tst=[mean_Rms_tst(1,:)' ,zeros(length(mean_Rms_tst),1)];
classes = svmclassify(svmStruct,tst,'showplot',true)
delete 'DS.wav'


fid = fopen('newfile.txt','w');
formatSpec = '%s , %d\r\n';
for x=1:numel(D)
    fprintf(fid,formatSpec,D(x).name,classes(x));
end
fclose(fid);