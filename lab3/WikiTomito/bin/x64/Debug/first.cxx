#encoding "utf-8"    // сообщаем парсеру о том, в какой кодировке написана грамматика
#GRAMMAR_ROOT S      // указываем корневой нетерминал грамматики

LName -> Word<gram="persn">;
FName -> Word<gram="famn">;
OName -> Word<gram="patrn">;
S -> LName<gnc-agr[1]>* interp (FIO.LName) FName<gnc-agr[1]> interp (FIO.FName) OName<gnc-agr[1]>* interp (FIO.OName);           // правило, выделяющее цепочку, состоящую из одного существительного
S -> FName<gnc-agr[1]> interp (FIO.FName) LName<gnc-agr[1]>* interp (FIO.LName) OName<gnc-agr[1]>* interp (FIO.OName); 
S -> FName<gnc-agr[1]> interp (FIO.FName) OName<gnc-agr[1]>* interp (FIO.OName) LName<gnc-agr[1]>* interp (FIO.LName);
S -> LName<gnc-agr[1]>* interp (FIO.LName)  OName<gnc-agr[1]>* interp (FIO.OName) FName<gnc-agr[1]> interp (FIO.FName); 
S -> OName<gnc-agr[1]>* interp (FIO.OName) LName<gnc-agr[1]>* interp (FIO.LName)  FName<gnc-agr[1]> interp (FIO.FName);
s -> OName<gnc-agr[1]>* interp (FIO.OName) FName<gnc-agr[1]> interp (FIO.FName) LName<gnc-agr[1]>* interp (FIO.LName);


//S -> LName<gnc-agr[1]>* FName<gnc-agr[1]> OName<gnc-agr[1]>*;//           // правило, выделяющее цепочку, состоящую из одного существительного
//S -> FName<gnc-agr[1]> LName<gnc-agr[1]>* OName<gnc-agr[1]>*; 
//S -> FName<gnc-agr[1]> OName<gnc-agr[1]>* LName<gnc-agr[1]>*;
//S -> LName<gnc-agr[1]>*  OName<gnc-agr[1]>* FName<gnc-agr[1]>; 
//S -> OName<gnc-agr[1]>* LName<gnc-agr[1]>*   FName<gnc-agr[1]>;
//S -> OName<gnc-agr[1]>* FName<gnc-agr[1]> LName<gnc-agr[1]>*;