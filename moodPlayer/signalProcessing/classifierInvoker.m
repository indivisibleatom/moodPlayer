function classifierInvoker(directory, file)
    import Classifier;
    classifier = Classifier(directory, file);
    classifier.classify();
    exit();
end