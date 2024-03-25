# -*- coding: utf-8 -*-

################################################################################
## Form generated from reading UI file 'main_formvDdVis.ui'
##
## Created by: Qt User Interface Compiler version 6.6.2
##
## WARNING! All changes made in this file will be lost when recompiling UI file!
################################################################################

from PySide6.QtCore import (QCoreApplication, QDate, QDateTime, QLocale,
    QMetaObject, QObject, QPoint, QRect,
    QSize, QTime, QUrl, Qt)
from PySide6.QtGui import (QBrush, QColor, QConicalGradient, QCursor,
    QFont, QFontDatabase, QGradient, QIcon,
    QImage, QKeySequence, QLinearGradient, QPainter,
    QPalette, QPixmap, QRadialGradient, QTransform)
from PySide6.QtWidgets import (QApplication, QFrame, QLabel, QSizePolicy,
    QWidget)
from utils.ui.custom_control import DragLabel

class Ui_main_form(object):
    def setupUi(self, main_form):
        if not main_form.objectName():
            main_form.setObjectName(u"main_form")
        main_form.setFixedSize(256, 256)
        self.label = QLabel(main_form)
        self.label.setObjectName(u"label")
        self.label.setGeometry(QRect(10, 10, 241, 41))
        font = QFont()
        font.setFamilies([u"JetBrains Mono"])
        font.setPointSize(12)
        font.setBold(False)
        self.label.setFont(font)
        self.label.setAlignment(Qt.AlignCenter)
        self.labelImg = DragLabel(main_form)
        self.labelImg.setObjectName(u"labelImg")
        self.labelImg.setGeometry(QRect(60, 60, 128, 128))
        self.labelImg.setFrameShape(QFrame.Box)
        self.labelImg.setLineWidth(3)
        self.labelImg.setAlignment(Qt.AlignCenter)

        self.retranslateUi(main_form)

        QMetaObject.connectSlotsByName(main_form)
    # setupUi

    def retranslateUi(self, main_form):
        main_form.setWindowTitle(QCoreApplication.translate("main_form", u"PNG2ICO", None))
        self.label.setText(QCoreApplication.translate("main_form", u"Drag PNG/ICO file here", None))
        self.labelImg.setText("")
    # retranslateUi

