function trainerInvoker(rootPath)
    import Trainer;
    trainer = Trainer(rootPath);
    trainer.train();
    exit();
end