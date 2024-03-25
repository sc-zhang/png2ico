from PySide6.QtGui import QPixmap
from PySide6.QtWidgets import QLabel, QMessageBox
from PIL import Image, ImageQt
from traceback import format_exc


class DragLabel(QLabel):

    def __init__(self, parent=None):
        super(DragLabel, self).__init__(parent)
        self.setAcceptDrops(True)
        self.img = ""

    def dragEnterEvent(self, event):
        if event.mimeData().hasUrls():
            event.acceptProposedAction()
        else:
            event.ignore()

    def dropEvent(self, event):
        if event.mimeData().hasUrls():
            img_path = event.mimeData().urls()[0].toLocalFile()
            if img_path.endswith('.png') or img_path.endswith(".ico"):
                try:
                    img = Image.open(img_path)
                    self.setPixmap(QPixmap.fromImage(ImageQt.ImageQt(img)).scaled(128, 128))
                    if img_path.endswith(".png"):
                        icon_sizes = [(16, 16), (32, 32), (64, 64), (128, 128), (256, 256)]
                        icon_path = img_path[:-4] + ".ico"
                        img.save(icon_path, format="ico", sizes=icon_sizes)
                        QMessageBox.information(self, "PNG2ICO", "Image saved: {}".format(icon_path))
                    else:
                        png_path = img_path[:-4] + ".png"
                        img.save(png_path, format="png", size=(256, 256))
                        QMessageBox.information(self, "PNG2ICO", "Image saved: {}".format(png_path))
                except Exception:
                    QMessageBox.critical(self, "Error", format_exc())
            else:
                QMessageBox.information(self, "PNG2ICO", "File format not supported")
            event.accept()
        else:
            event.ignore()
