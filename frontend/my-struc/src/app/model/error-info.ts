import { HttpServiceError } from '../httpService/HttpServiceError';
export class ErrorInfo {
  public readonly source: string;
  public readonly message: string|null = null;
  public readonly error: string|HttpServiceError;

  constructor(source: string, msg: string|HttpServiceError) {
    this.source = source;
    if (typeof msg === 'string') {
      this.message = msg;
    } else
    if (msg instanceof HttpServiceError) {
      this.message = msg.status != null ? `${msg.status} ${msg.statusText} - ${msg.body}` : msg.body;
    }

    this.error = msg;
  }
}
