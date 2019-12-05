export declare class HttpServiceError {
  status: number | null;
  statusText: string | null;
  body: string;
  responseBody: any;
  constructor(status: number | null, statusText: string | null, body: string, responseBody: any);
}
