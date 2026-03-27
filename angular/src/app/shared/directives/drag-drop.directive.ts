import { Directive, EventEmitter, HostBinding, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[appDragDrop]',
  standalone: false
})
export class DragDropDirective {
  // Event emitted when files are dropped
  @Output() fileDropped = new EventEmitter<FileList>();

  // Use this to bind a CSS class when drag is over
  @HostBinding('class.drag-over') isDragOver = false;

  constructor() {}

  // Dragover listener
  @HostListener('dragover', ['$event']) onDragOver(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.isDragOver = true;
  }

  // Dragleave listener
  @HostListener('dragleave', ['$event']) onDragLeave(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.isDragOver = false;
  }

  // Drop listener
  @HostListener('drop', ['$event']) onDrop(evt: DragEvent) {
    evt.preventDefault();
    evt.stopPropagation();
    this.isDragOver = false;

    if (evt.dataTransfer && evt.dataTransfer.files) {
      if (evt.dataTransfer.files.length > 0) {
        this.fileDropped.emit(evt.dataTransfer.files);
      }
    }
  }
}
