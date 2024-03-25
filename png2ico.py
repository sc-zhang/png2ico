#!/usr/bin/env python3
import sys
from os import path
from PySide6.QtWidgets import QApplication
from PySide6.QtGui import QIcon
from PySide6.QtCore import QCoreApplication, Qt
from utils.main.main import PNG2ICO


if __name__ == "__main__":
    QCoreApplication.setAttribute(Qt.AA_ShareOpenGLContexts)
    app = QApplication([])
    icon_path = path.join(path.dirname(path.abspath(__file__)), "utils/resources/icon.png")
    app.setWindowIcon(QIcon(icon_path))
    main_window = PNG2ICO()
    main_window.show()
    sys.exit(app.exec())
