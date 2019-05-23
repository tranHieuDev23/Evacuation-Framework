\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{my_style}
\LoadClass[12pt, a4paper]{article}

\RequirePackage{titlesec}
\RequirePackage[utf8]{vietnam}
% \RequirePackage[utf8]{inputenc}
% \RequirePackage[T1]{fontenc}
% \RequirePackage{latexsym}
% \RequirePackage{amsmath}
\RequirePackage{graphicx}
\RequirePackage{xcolor}
% \RequirePackage{setspace}
% \RequirePackage{geometry}
% \RequirePackage{marginnote}
% \RequirePackage{caption}
% \RequirePackage{siunitx}
% \RequirePackage{eurosym}
% \RequirePackage{colortbl}
% \RequirePackage{array}
% \RequirePackage{sectsty}
% \RequirePackage[nottoc]{tocbibind}
% \RequirePackage{float}
% \RequirePackage{wrapfig}
% \RequirePackage{vmargin}

\definecolor{Sapienza}{RGB}{130,36,51}
\definecolor{Rosso}{RGB}{232,61,61}
\definecolor{Viola}{RGB}{188,30,188}
\definecolor{Celeste}{RGB}{34,186,211}
\definecolor{Arancione}{RGB}{232,181,19}
\definecolor{Blu}{RGB}{8,144,213}
\definecolor{Azzurro}{RGB}{57,186,238}
\definecolor{Sapienza}{RGB}{111,10,25}
\definecolor{gold}{RGB}{199,153,0}
\RequirePackage{hyperref}
% \hypersetup{colorlinks=true,linkcolor=black,allbordercolors=white}
% \RequirePackage{enumitem} 
% \renewcommand\labelitemi{\textbullet} 
% \RequirePackage{pdflscape}
% \RequirePackage{pgfplots}
% \usetikzlibrary{shapes}
% \RequirePackage{pgfplots}
% \usetikzlibrary{intersections,positioning,calc}
% \RequirePackage{tkz-euclide,tkz-fct}
%     \usetkzobj{all}
% \usetikzlibrary{angles,quotes}
% \RequirePackage{lmodern}
% \RequirePackage{titlesec}
\RequirePackage{tikz}
% \usetikzlibrary{shapes}
% \usetikzlibrary{arrows,positioning}
% \RequirePackage{amsmath,bm}

% \RequirePackage{mathspec}
% \RequirePackage{xltxtra,xunicode}

\renewcommand{\maketitle}{
\thispagestyle{empty}
\begin{figure}
\vspace{-1cm}
\centering
\includegraphics[width=0.6\textwidth]{hustlogo.png}\\[1cm]
\end{figure}

\begin{center}
\textcolor{Blu}{\bfseries\large\fontsize{5mm}{7mm}\selectfont
Viện Công Nghệ Thông Tin và Truyền Thông\\Lớp KSTN-CNTT K62\\[1cm]}
\end{center}
\begin{center}
\Large\bfseries Báo cáo môn học Kĩ Thuật Lập Trình\\[0.5cm] \bfseries
Bài Toán Tìm Đường Di Tản Trong Tòa Nhà Lớn Khi Xảy Ra Hỏa Hoạn\\[0.3cm]\normalsize Subtitle \\ 
\end{center}


\vspace{3cm}



\vspace{-1.5cm}
\begin{minipage}[t]{7cm}
\begin{flushleft} \large
\emph{Author:}\\
\textbf{Trương Ngọc Giang \\
        Trần Minh Hiếu \\
        Trương Quang Khánh \\
        Nguyễn Mai Phương \\}
\end{flushleft}
\end{minipage}
~
\begin{minipage}[t]{8cm}
\begin{flushleft} \large
\emph{Teacher:} \\
\emph{PGS.TS} \textbf{Huỳnh Quyết Thắng }
\end{flushleft}
\end{minipage}
\begin{center}
\vfill
{\sc Academic year 2019}
\end{center}
\newpage
}

\titleformat{\section}[display]
  {\LARGE\normalfont\em\bfseries\color{Blu}}
  {\filleft
  \begin{tikzpicture}
  \node[
    outer sep=0pt,
    text width=2.5cm,
    minimum height=3cm,
    fill=white,
    font=\color{Blu}\fontsize{50}{0}\selectfont,
    align=center
    ] (num) {\thesection};
  \node[
    rotate=90,
    anchor=south,
    font=\color{black}\large\normalfont
    ] at ([xshift=-5pt]num.west) {\Huge\textsc{}};  
  \end{tikzpicture}%
    }{12pt}
    {}
    [\titlerule]

    \newcommand*\numb[1]{%
\begin{tikzpicture}[baseline=-0.7ex]
\node[
  outer sep=0pt,
      text width=0.6cm,
      minimum height=0.6cm,
      fill=Blu,
      font=\color{white}\fontsize{12}{20}\selectfont,
      align=center
      ] (num) {\thesubsection};
\end{tikzpicture}%
}
\titleformat{\subsection}
  {\normalfont\color{Blu}\large\sc\sffamily}{\numb{\thesubsection}}{0.8em}{}

%--Intestazione subsubsection--
\newcommand*\stocaz[1]{%
\begin{tikzpicture}[baseline=-0.7ex]
\node[
  outer sep=0pt,
      text width=0.8cm,
      minimum height=0.5cm,
      fill=Blu,
      font=\color{white}\fontsize{10}{20}\selectfont,
      align=center
      ] (num) {\thesubsubsection};
\end{tikzpicture}%
}
\titleformat{\subsubsection}
  {\normalfont\color{Blu}\bfseries\sc\sffamily}{\stocaz{\thesubsubsection}}{0.8em}{}
