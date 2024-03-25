from utils.ui import ui_main_form
from PySide6.QtWidgets import QWidget


class PNG2ICO(QWidget):
    def __init__(self):
        super(PNG2ICO, self).__init__()
        self.ui = ui_main_form.Ui_main_form()
        self.ui.setupUi(self)
